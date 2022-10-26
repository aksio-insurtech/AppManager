// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { useParams } from 'react-router-dom';
import { Routes } from './Routes';
import { IngressById } from 'API/applications/environments/ingresses/IngressById';


export const Ingress = () => {
    const { ingressId } = useParams();
    const [selectedTab, setSelectedTab] = useState("0");
    const [ingress] = IngressById.use({ ingressId: ingressId! });

    return (
        <TabContext value={selectedTab}>

            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="General" value="0" />
                    <Tab label="Authentication" value="1" />
                    <Tab label="Routes" value="2" />
                </TabList>
            </Box>
            <TabPanel value="0"></TabPanel>
            <TabPanel value="1"></TabPanel>
            <TabPanel value="2"><Routes ingress={ingress.data}/></TabPanel>
        </TabContext>
    );
};
