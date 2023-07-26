// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { ViteEjsPlugin } from 'vite-plugin-ejs';
import path from 'path';

export default defineConfig({
    build: {
        outDir: './wwwroot',
        assetsDir: ''
    },
    plugins: [
        react(),
        ViteEjsPlugin((viteConfig) => {
            return {
                root: viteConfig.root,
                domain: 'apps.aksio.no',
                title: 'Aksio App Manager',
            };
        })
    ],
    resolve: {
        alias: {
            '@': path.resolve('./'),
            'API': path.resolve('./API'),
            'Components': path.resolve('./Components')
        },
        dedupe: ['react', 'react-dom', 'react-router-dom', '@mui/material', '@mui/icons-material']
    },

    server: {
        port: 9100,
        proxy: {
            '/api': {
                target: 'http://localhost:5100',
                ws: true
            },
            '/swagger': {
                target: 'http://localhost:5100'
            }
        }
    }
});
