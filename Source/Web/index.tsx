// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import ReactDOM from 'react-dom';
import { createRoot } from 'react-dom/client';

import './theme';

import { App } from './App';
import { BrowserRouter } from 'react-router-dom';
import { CssBaseline, ThemeProvider } from '@mui/material';
import { ModalProvider } from '@aksio/cratis-mui';

import { theme } from './theme';

const root = createRoot(document.getElementById('root')!);
root.render(
    <BrowserRouter>
        <ThemeProvider theme={theme}>
            <ModalProvider>
                <CssBaseline />
                <App />
            </ModalProvider>
        </ThemeProvider>
    </BrowserRouter>
);
