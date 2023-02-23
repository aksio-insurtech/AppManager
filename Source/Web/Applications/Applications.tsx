// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { Route, Routes, useNavigate, useLocation, matchPath } from 'react-router-dom';
import { CreateApplicationDialog } from './CreateApplicationDialog';
import { CreateApplication } from 'API/applications/CreateApplication';
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
import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import { ApplicationItemWithArtifacts } from './ApplicationItemWithArtifacts';
import { Tenants } from './Tenants/Tenants';
import { Ingress } from './Ingresses/Ingress';
import { Cratis } from './Cratis/Cratis';
import { EnvironmentForApplication } from 'API/applications/environments/EnvironmentForApplication';
import { Settings } from './ApplicationEnvironmentSettings/Settings';
import { Certificates } from './Certificates/Certificates';
import { Consolidations } from './Consolidations/Consolidations';

export const Applications = () => {
    const [currentApplicationId, setCurrentApplicationId] = useState<string>();
    const [currentEnvironment, setCurrentEnvironment] = useState<string>();
    const [currentMicroservice, setCurrentMicroservice] = useState<string>();
    const [currentDeployable, setCurrentDeployable] = useState<string>();
    const navigate = useNavigate();
    const location = useLocation();
    const [applicationsHierarchy] = ApplicationsHierarchy.use();
    const [applicationEnvironment, performApplicationEnvironmentQuery] = EnvironmentForApplication.use();

    const routes: string[] = [
        '/applications/:applicationId',
        '/applications/:applicationId/environments/:environmentId',
        '/applications/:applicationId/environments/:environmentId/ingresses/:ingressId',
        '/applications/:applicationId/environments/:environmentId/settings',
        '/applications/:applicationId/environments/:environmentId/consolidations',
        '/applications/:applicationId/environments/:environmentId/cratis',
        '/applications/:applicationId/environments/:environmentId/tenants',
        '/applications/:applicationId/environments/:environmentId/certificates',
        '/applications/:applicationId/environments/:environmentId/microservices/:microserviceId',
        '/applications/:applicationId/environments/:environmentId/microservices/:microserviceId/deployables/:deployableId'
    ];

    useEffect(() => {
        const match = routes.map(_ => matchPath({ path: _ }, location.pathname)).filter(_ => _ !== null);
        if (match?.length == 1) {
            const params = match[0]?.params || {};

            if (currentEnvironment != params.environmentId) {
                performApplicationEnvironmentQuery({ applicationId: params.applicationId!, environmentId: params.environmentId! });
            }

            setCurrentApplicationId(params.applicationId);
            setCurrentEnvironment(params.environmentId);
            setCurrentMicroservice(params.microserviceId);
            setCurrentDeployable(params.deployableId);
        } else {
            setCurrentApplicationId(undefined);
            setCurrentEnvironment(undefined);
            setCurrentMicroservice(undefined);
            setCurrentDeployable(undefined);
        }
    }, [location.pathname]);

    let currentApplication: ApplicationHierarchyForListing | undefined;
    if (currentApplicationId && applicationsHierarchy.data.length > 0) {
        currentApplication = applicationsHierarchy.data.find(_ => _.id == currentApplicationId);
    }

    const [showCreateApplication] = useModal(
        'Create application',
        ModalButtons.OkCancel,
        CreateApplicationDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new CreateApplication();
                command.applicationId = Guid.create().toString();
                command.name = output!.name;
                command.sharedAzureSubscriptionId = output!.sharedAzureSubscriptionId;
                await command.execute();
            }
        }
    );

    return (
        <>
            <Grid container sx={{ height: '100%' }}>
                <Grid item xs={2}>
                    <Paper elevation={1} sx={{ width: '100%', height: '100%' }}>
                        <ApplicationsNav>
                            <ListItemButton component="a" onClick={() => navigate('/applications')}>
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

                            {!currentApplication ?
                                applicationsHierarchy.data.map(application => {
                                    return (
                                        <span key={application.id}>
                                            <ApplicationItem application={application} />
                                            <Divider />
                                        </span>
                                    );
                                })
                                : <ApplicationItemWithArtifacts
                                    application={currentApplication}
                                    environmentId={currentEnvironment} />
                            }
                        </ApplicationsNav>
                    </Paper>
                </Grid>
                <Grid item xs={10}>
                    <Paper elevation={0} sx={{ height: '100%' }}>
                        <Routes>
                            <Route path=':applicationId' element={<Application />} />
                            <Route path=':applicationId/environments/:environmentId' element={<Application />} />
                            <Route path=':applicationId/environments/:environmentId/ingresses/:ingressId' element={<Ingress />} />
                            <Route path=':applicationId/environments/:environmentId/settings' element={<Settings />} />
                            <Route path=':applicationId/environments/:environmentId/consolidations' element={<Consolidations />} />
                            <Route path=':applicationId/environments/:environmentId/cratis' element={<Cratis environment={applicationEnvironment.data} />} />
                            <Route path=':applicationId/environments/:environmentId/tenants' element={<Tenants />} />
                            <Route path=':applicationId/environments/:environmentId/certificates' element={<Certificates />} />
                            <Route path=':applicationId/environments/:environmentId/microservices/:microserviceId' element={<Microservice />} />
                            <Route path=':applicationId/environments/:environmentId/microservices/:microserviceId/deployables/:deployableId' element={<Deployable />} />
                        </Routes>
                    </Paper>
                </Grid>
            </Grid>
        </>
    );
};
