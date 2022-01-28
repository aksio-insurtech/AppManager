// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import ReactDOM from 'react-dom';

import './index.scss';
import './Styles/theme';

import { App } from './App';
import { ModalProvider } from '@aksio/cratis-fluentui';
import { BrowserRouter } from 'react-router-dom';

ReactDOM.render(
    <BrowserRouter>
        <ModalProvider>
            <App />
        </ModalProvider>
    </BrowserRouter>,
    document.getElementById('root')
);
