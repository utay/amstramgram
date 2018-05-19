using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Api.Controllers;
using Api.Models;
using Core;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Amstramgram.Tests.Unit.Api.Controllers
{
    [TestClass]
    public class GraphQLControllerShould
    {
        private GraphQLController _graphqlController { get; set; }
        private Core.Data.IUserRepository repoUser;

        public GraphQLControllerShould()
        {
            // Given
            var documentExecutor = new Mock<IDocumentExecuter>();
            documentExecutor.Setup(x => x.ExecuteAsync(It.IsAny<ExecutionOptions>())).Returns(Task.FromResult(new ExecutionResult()));
            var schema = new Mock<ISchema>();
            var logger = new Mock<ILogger<GraphQLController>>();


            List<Core.Models.User> users = new List<Core.Models.User>
                {
                    new Core.Models.User {Id= 1, Nickname = "test", Email = "test@dqsdqs.com", Lastname = "test", Firstname = "test"},

                    new Core.Models.User {Id= 2, Nickname = "test", Email = "test@fdsfsdfsd.com", Lastname = "test", Firstname = "test"}
            };

            Mock<Core.Data.IUserRepository> mockProductRepository = new Mock<Core.Data.IUserRepository>();            
            mockProductRepository.Setup(mr => mr.GetAll()).Returns(Task.Run(() => users));
            mockProductRepository.Setup(mr => mr.Get(It.IsAny<long>())).Returns(Task.Run(() => users[0]));

            mockProductRepository.Setup(mr => mr.Add(It.IsAny<Core.Models.User>())).Returns(
                (Core.Models.User target) =>
                {
                    return users[0];   
                });

            this.repoUser = mockProductRepository.Object;

            _graphqlController = new GraphQLController(documentExecutor.Object, schema.Object, logger.Object, null);
        }

        
        [TestMethod]
        public void ReturnNotNullViewResult()
        {
            // When
            var result = _graphqlController.Index() as ViewResult;

            // Then
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async void CreateUser()
        {
            // Given
            var query = new GraphQLQuery { Query = @"mutation {
                createUser(user:{
                   nickname:'test',
                   email:'test@gmail.com',
                   password: 'test',
                   firstname: 'test',
                   lastname: 'test',
                   picture: 'http://cdn-europe1.new2.ladmedia.fr/var/europe1/storage/images/le-lab/francois-hollande-ne-va-pas-partir-aux-champignons-fin-mai-selon-jean-christophe-cambadelis-2980766/33011039-1-fre-FR/Francois-Hollande-ne-va-pas-partir-aux-champignons-fin-mai-selon-Jean-Christophe-Cambadelis.jpg',
                   phone: '0600000000',
                   gender: 'male',
                   description: 'Curious and passionate developer. Student at EPITA.',
                   private:true
                                })
            }" };

            // When
            var result = await _graphqlController.Post(query);

            // Then
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Core.Models.User));
        }

        [TestMethod]
        public async void CreatePictures()
        {
            // Given
            var query = new GraphQLQuery { Query = @"mutation {
                        createPicture(picture: {
                             image: 'fds',
                             description: '${data.description}',
                             userId: 1,
                             tags: ['fsdfs', 'sdfsdfds'],
                             color: ''
                        })}" };

            // When
            var result = await _graphqlController.Post(query);

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async void CreateLike()
        {
            // Given
            var query = new GraphQLQuery { Query = @"mutation {
                        createPicture(picture: {
                             image: 'fds',
                             description: '${data.description}',
                             userId: 1,
                             tags: ['fsdfs', 'sdfsdfds'],
                             color: ''
                        })}" };

            // When
            var result = await _graphqlController.Post(query);

            // Then
            Assert.IsNotNull(result);
        }
    }
}
