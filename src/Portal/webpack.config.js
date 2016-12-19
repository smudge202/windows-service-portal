"use strict";

module.exports = {
    entry: "./jssrc/app.jsx",
    output: {
        filename: "./wwwroot/js/app.js"
    },
    devServer: {
        contentBase: ".",
        host: "localhost",
        port: 9000
    },
    module: {
        loaders: [
            {
                test: /\.jsx?$/,
                loader: "babel-loader",
                query: {
                    presets: ["react", "es2015"]
                }
            }
        ]
    }
};