// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { default as styles } from './App.module.scss';
import { Navigation } from './Navigation';
import { Home } from './Home';
import { Header } from './Header';
import { Applications } from './Applications/Applications';
import { Organization } from './Organizations/Organization';

export const App = () => {
    return (
        <div className={styles.appContainer}>
            <div className={styles.header}>
                <Header />
            </div>
            <div className={styles.content}>
                <div className={styles.navigationContainer}>
                    <div className={styles.navigationBar}>
                        <Navigation />
                    </div>
                    <div style={{ width: '100%' }}>
                        <Routes>
                            <Route path="/" element={<Home />} />
                            <Route path="/applications/*" element={<Applications />} />
                            <Route path="/organization/*" element={<Organization />} />
                        </Routes>
                    </div>
                </div>
            </div>
        </div>
    );
};
