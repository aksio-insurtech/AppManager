// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { General } from './General';


export const Cratis = () => {
    const [selectedTab, setSelectedTab] = useState("0");

    return (
        <TabContext value={selectedTab}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="General" value="0" />
                </TabList>
            </Box>
            <TabPanel value="0"><General/></TabPanel>
        </TabContext>
    );
};
