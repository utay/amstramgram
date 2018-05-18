import React from 'react';
import ReactDOM from 'react-dom';
import GraphiQL from 'graphiql';
import fetch from 'isomorphic-fetch';
import 'graphiql/graphiql.css';
import './app.css';

function getCookie(name) {
	let pairs = document.cookie.split(";");
	let cookies = {};
	for (let i = 0; i < pairs.length; i++) {
		let pair = pairs[i].split("=");
		if (pair[0] === name)
			return pair[1];
		cookies[(pair[0] + '').trim()] = unescape(pair[1]);
	}
	return undefined;
}

function graphQLFetcher(graphQLParams) {
    return fetch(window.location.origin + '/graphql', {
        method: 'post',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(graphQLParams)
    }).then(response => response.json());
}

ReactDOM.render(<GraphiQL fetcher={graphQLFetcher}/>, document.getElementById('app'));
