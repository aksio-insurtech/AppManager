// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { Route, Routes, useNavigate, useLocation, matchPath } from 'react-router-dom';
import { CreateApplicationDialog } from './CreateApplicationDialog';
import { Create as CreateApplication } from 'API/applications/Create';
import { Guid } from '@aksio/cratis-fundamentals';
import { ApplicationsHierarchy } from 'API/applications/ApplicationsHierarchy';
import { Application } from './Application';

import * as icons from '@mui/icons-material';
import { Microservice } from './Microservices/Microservice';
import { Deployable } from './Microservices/Deployables/Deployable';
import { Box, Divider, Grid, ListItem, ListItemButton, ListItemIcon, ListItemText, Paper, Menu, MenuItem } from '@mui/material';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-mui';
import { ApplicationsNav } from './ApplicationsNav';
import { ListItemActionButton } from './ListItemActionButton';
import { ApplicationItem } from './ApplicationItem';


export const Applications = () => {
    const [selectedNav, setSelectedNav] = useState('');
    const [currentApplication, setCurrentApplication] = useState<string>();
    const [currentMicroservice, setCurrentMicroservice] = useState<string>();
    const [currentDeployable, setCurrentDeployable] = useState<string>();
    const navigate = useNavigate();
    const location = useLocation();
    const [applicationsHierarchy] = ApplicationsHierarchy.use();

    const routes: string[] = [
        '/applications/:applicationId',
        '/applications/:applicationId/environments/:environmentId/microservices/:microserviceId',
        '/applications/:applicationId/environments/:environmentId/microservices/:microserviceId/deployables/:deployableId'
    ];

    useEffect(() => {
        const match = routes.map(_ => matchPath({ path: _ }, location.pathname)).filter(_ => _ !== null);
        if (match?.length == 1) {
            const params = match[0]?.params || {};

            setCurrentApplication(params.applicationId);
            setCurrentMicroservice(params.microserviceId);
            setCurrentDeployable(params.deployableId);

            setSelectedNav(params.deployableId || params.microserviceId || params.applicationId || '');
        }
    }, [location.pathname]);

    const [showCreateApplication] = useModal(
        'Create application',
        ModalButtons.OkCancel,
        CreateApplicationDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new CreateApplication();
                command.applicationId = Guid.create().toString();
                command.name = output!.name;
                command.azureSubscriptionId = output!.azureSubscription;
                command.cloudLocation = output!.cloudLocation;
                await command.execute();
            }
        }
    );

    const open = true;

    return (
        <>
            <Grid container sx={{ height: '100%' }}>
                <Grid item xs={2}>
                    <Paper elevation={1} sx={{ width: '100%', height: '100%' }}>
                        <ApplicationsNav>
                            <ListItemButton component="a">
                                <ListItemIcon><icons.Apps /></ListItemIcon>
                                <ListItemText
                                    sx={{ my: 0 }}
                                    primaryTypographyProps={{
                                        fontSize: 20,
                                        fontWeight: 'medium',
                                        letterSpacing: 0
                                    }}>
                                    Applications
                                </ListItemText>
                                <ListItemActionButton title="Add Application" icon={<icons.Add />} onClick={showCreateApplication} />
                            </ListItemButton>

                            <Divider />

                            {applicationsHierarchy.data.map(application => {
                                return (
                                    <span key={application.id}>
                                        <ApplicationItem application={application} />
                                        <Divider />
                                        <Box
                                            sx={{
                                                bgcolor: open ? 'rgba(71, 98, 130, 0.2)' : null,
                                                pb: open ? 2 : 0,
                                            }}>

                                            <ListItemButton sx={{
                                                px: 3,
                                                pt: 2.5,
                                                pb: open ? 0 : 2.5,
                                                '&:hover, &focus': { '& svg': { opacity: open ? 1 : 0 } }
                                            }}>
                                                <ListItemIcon><icons.Input /></ListItemIcon>
                                                <ListItemText>Ingress</ListItemText>
                                            </ListItemButton>
                                        </Box>

                                        {application.microservices?.map(microservice => {
                                            return (
                                                <Box key={microservice.name}
                                                    sx={{
                                                        bgcolor: open ? 'rgba(71, 98, 130, 0.2)' : null,
                                                        pb: open ? 2 : 0,
                                                    }}>

                                                    <ListItemButton sx={{
                                                        px: 3,
                                                        pt: 2.5,
                                                        pb: open ? 0 : 2.5,
                                                        '&:hover, &focus': { '& svg': { opacity: open ? 1 : 0 } }
                                                    }}>
                                                        <ListItemIcon><icons.Cabin /></ListItemIcon>
                                                        <ListItemText>Members</ListItemText>
                                                    </ListItemButton>
                                                </Box>
                                            );
                                        })}
                                    </span>
                                );
                            })}
                        </ApplicationsNav>
                    </Paper>
                </Grid>
                <Grid item xs={10}>
                    <Paper elevation={0} sx={{ height: '100%' }}>
                        <Routes>
                            <Route path=':applicationId' element={<Application />} />
                            <Route path=':applicationId/environments/:environmentId/microservices/:microserviceId' element={<Microservice />} />
                            <Route path=':applicationId/environments/:environmentId/microservices/:microserviceId/deployables/:deployableId' element={<Deployable />} />
                        </Routes>
                    </Paper>
                </Grid>
            </Grid>
        </>
    );
};
