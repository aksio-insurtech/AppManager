const path = require('path');

const webpack = require('@aksio/cratis-webpack/frontend');
module.exports = (env, argv) => {
    const isWebDevServer = (process.env.WEBPACK_DEV_SERVER || false) === 'true' ? true : false;
    const basePath = isWebDevServer ? '/' : process.env.base_path || '';
    return webpack(env, argv, basePath, config => {
        config.resolve.alias.API = path.resolve('./API');
        config.devServer.port = 9100;
        config.devServer.proxy = {
            '/api': {
                target: 'http://localhost:5100',
                ws: true
            },
            '/swagger': 'http://localhost:5100'
        };
        config.resolve.alias.API = path.resolve('./API');
        config.resolve.alias.Components = path.resolve('./Components');
    }, 'AppManager');
};
