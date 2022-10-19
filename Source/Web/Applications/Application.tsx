// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { General } from './General';
import { Tenants } from './Tenants';
import { GetApplication } from 'API/applications/GetApplication';
import { useParams } from 'react-router-dom';
import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';

export const Application = () => {
    const { applicationId } = useParams();
    const [application] = GetApplication.use({ applicationId: applicationId! });
    const [selectedTab, setSelectedTab] = useState("0");

    return (
        <>
            <TabContext value={selectedTab}>

                <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                    <TabList onChange={(e, value) => setSelectedTab(value)}>
                        <Tab label="First" value="0" />
                        <Tab label="Second" value="1" />
                        <Tab label="Third" value="2" />
                    </TabList>
                </Box>
                <TabPanel value="0">First</TabPanel>
                <TabPanel value="1">Second</TabPanel>
                <TabPanel value="2">Third</TabPanel>

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
