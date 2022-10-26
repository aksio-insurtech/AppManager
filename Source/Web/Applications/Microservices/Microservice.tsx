// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GetMicroservice } from 'API/applications/environments/microservices/GetMicroservice';
import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Variable, Variables } from '../Variables/Variables';
import { Secrets } from '../Secrets/Secrets';
import { Deployables } from './Deployables/Deployables';
import { ConfigFiles } from '../ConfigFiles/ConfigFiles';
import { General } from './General';
import { RouteParams, useRouteParams } from '../RouteParams';
import { SetEnvironmentVariableForMicroservice } from 'API/applications/environments/microservices/SetEnvironmentVariableForMicroservice';

export const Microservice = () => {
    const { applicationId, environmentId, microserviceId } = useRouteParams();
    const [microservice, performMicroserviceQuery] = GetMicroservice.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!
    });
    const [selectedTab, setSelectedTab] = useState("0");

    const variableSet = async (variable: Variable, context: RouteParams) => {
        const command = new SetEnvironmentVariableForMicroservice();
        command.applicationId = applicationId;
        command.environmentId = environmentId!;
        command.microserviceId = context.microserviceId!;
        command.key = variable.key;
        command.value = variable.value;
        await command.execute();
    };

    return (
        <TabContext value={selectedTab}>

            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="General" value="0" />
                    <Tab label="Deployables" value="1" />
                    <Tab label="Config Files" value="2" />
                    <Tab label="Variables" value="3" />
                    <Tab label="Secrets" value="4" />
                </TabList>
            </Box>
            <TabPanel value="0"><General microservice={microservice.data} /></TabPanel>
            <TabPanel value="1"><Deployables /></TabPanel>
            <TabPanel value="2"><ConfigFiles /></TabPanel>
            <TabPanel value="3"><Variables onVariableSet={variableSet} variables={[]}/></TabPanel>
            <TabPanel value="4"><Secrets /></TabPanel>
        </TabContext>
    );
};
