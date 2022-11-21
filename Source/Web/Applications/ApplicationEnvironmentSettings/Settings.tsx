// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Variable, Variables } from '../Variables/Variables';
import { Secrets } from '../Secrets/Secrets';
import { ConfigFiles } from '../ConfigFiles/ConfigFiles';
import { RouteParams, useRouteParams } from '../RouteParams';
import { SetEnvironmentVariableForApplicationEnvironment } from 'API/applications/environments/SetEnvironmentVariableForApplicationEnvironment';
import { EnvironmentVariablesForApplicationEnvironmentId } from 'API/applications/environments/EnvironmentVariablesForApplicationEnvironmentId';

export const Settings = () => {
    const { applicationId, environmentId, microserviceId } = useRouteParams();
    const [selectedTab, setSelectedTab] = useState("0");
    const [environmentVariablesQuery] = EnvironmentVariablesForApplicationEnvironmentId.use({ environmentId: environmentId! });
    const environmentVariables = environmentVariablesQuery.data?.variables ?? [];

    const variableSet = async (variable: Variable, context: RouteParams) => {
        const command = new SetEnvironmentVariableForApplicationEnvironment();
        command.applicationId = applicationId;
        command.environmentId = environmentId!;
        command.key = variable.key;
        command.value = variable.value;
        await command.execute();
    };

    return (
        <TabContext value={selectedTab}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="Config Files" value="0" />
                    <Tab label="Variables" value="1" />
                    <Tab label="Secrets" value="2" />
                </TabList>
            </Box>
            <TabPanel value="0"><ConfigFiles /></TabPanel>
            <TabPanel value="1"><Variables onVariableSet={variableSet} variables={environmentVariables}/></TabPanel>
            <TabPanel value="2"><Secrets /></TabPanel>
        </TabContext>
    );
};
