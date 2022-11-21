// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Home } from './Home';
import { Header } from './Header';
import { Applications } from './Applications/Applications';
import { Settings } from './Settings/Settings';

export const App = () => {
    return (
        <>
            <Header />
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/applications/*" element={<Applications />} />
                <Route path="/settings/*" element={<Settings />} />
            </Routes>
        </>
    );
};
