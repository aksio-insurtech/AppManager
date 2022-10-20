// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GetApplication } from 'API/applications/GetApplication';
import { useParams } from 'react-router-dom';
import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Environments } from './Environments/Environments';

export const Application = () => {
    const { applicationId } = useParams();
    const [application] = GetApplication.use({ applicationId: applicationId! });
    const [selectedTab, setSelectedTab] = useState("0");

    return (
        <>
            <TabContext value={selectedTab}>

                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                    <TabList onChange={(e, value) => setSelectedTab(value)}>
                        <Tab label="Environments" value="0" />
                        <Tab label="Config Files" value="1" />
                        <Tab label="Variables" value="2" />
                    </TabList>
                </Box>
                <TabPanel value="0"><Environments /></TabPanel>
                <TabPanel value="1"></TabPanel>
                <TabPanel value="2"></TabPanel>
            </TabContext>

        </>
        // <Stack style={{ height: '100%' }}>
        //     <Stack.Item>
        //         <Pivot>
        //             <PivotItem headerText="General" itemIcon="Settings">
        //                 { application.data.id && <General application={application.data} /> }
        //             </PivotItem>
        //             <PivotItem headerText="Tenants" itemIcon="Quantity">
        //                 <Tenants />
        //             </PivotItem>
        //             <PivotItem headerText="Environment" itemIcon="Variable">
        //             </PivotItem>
        //             <PivotItem headerText="Secrets" itemIcon="Encryption">
        //             </PivotItem>
        //         </Pivot>
        //     </Stack.Item>
        // </Stack>
    );
};
