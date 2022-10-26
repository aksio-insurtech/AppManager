// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GetApplication } from 'API/applications/GetApplication';
import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Environments } from './Environments/Environments';
import { Variables, Variable } from './Variables/Variables';
import { Secrets } from './Secrets/Secrets';
import { ConfigFiles } from './ConfigFiles/ConfigFiles';
import { useRouteParams, RouteParams } from './RouteParams';
import { SetEnvironmentVariableForApplication } from 'API/applications/SetEnvironmentVariableForApplication';

export const Application = () => {
    const { applicationId } = useRouteParams();
    const [application] = GetApplication.use({ applicationId: applicationId! });
    const [selectedTab, setSelectedTab] = useState("0");

    const variableSet = async (variable: Variable, context: RouteParams) => {
        const command = new SetEnvironmentVariableForApplication();
        command.applicationId = context.applicationId;
        command.key = variable.key;
        command.value = variable.value;
        await command.execute();
    };

    return (
        <TabContext value={selectedTab}>

            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="General" value="0" />
                    <Tab label="Environments" value="1" />
                    <Tab label="Config Files" value="2" />
                    <Tab label="Variables" value="3" />
                    <Tab label="Secrets" value="4" />
                </TabList>
            </Box>
            <TabPanel value="0"></TabPanel>
            <TabPanel value="1"><Environments /></TabPanel>
            <TabPanel value="2"><ConfigFiles /></TabPanel>
            <TabPanel value="3"><Variables onVariableSet={variableSet} /></TabPanel>
            <TabPanel value="4"><Secrets /></TabPanel>
        </TabContext>
    );
};
