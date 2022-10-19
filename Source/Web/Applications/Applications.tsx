// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { Route, Routes, useNavigate, useParams, useLocation, matchPath } from 'react-router-dom';
import { CreateApplicationDialog } from './CreateApplicationDialog';
import { Create as CreateApplication } from 'API/applications/Create';
import { Guid } from '@aksio/cratis-fundamentals';
import { ApplicationsHierarchy } from 'API/applications/ApplicationsHierarchy';
import { Application } from './Application';
import { CreateMicroserviceDialog } from './CreateMicroserviceDialog';
import { Create as CreateMicroservice } from 'API/applications/microservices/Create';
import { Create as CreateDeployable } from 'API/applications/microservices/deployables/Create';
import { CreateDeployableDialog } from './CreateDeployableDialog';

import { ExpandMore, ChevronRight, Add } from '@mui/icons-material';
import { Microservice } from './Microservices/Microservice';
import { Deployable } from './Microservices/Deployables/Deployable';
import { TreeItem, TreeView } from '@mui/lab';
import { Box, Drawer, Grid, IconButton } from '@mui/material';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-mui';


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
        '/applications/:applicationId/microservices/:microserviceId',
        '/applications/:applicationId/microservices/:microserviceId/deployables/:deployableId'
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
        (result, output) => {
            if (result == ModalResult.success) {
                debugger;
            }
        }
    );

    return (
        <>
            <IconButton title="Add Application" onClick={showCreateApplication}><Add /></IconButton>
            <Grid container>
                <Grid item xs={1}>
                    <TreeView
                        defaultCollapseIcon={<ExpandMore />}
                        defaultExpandIcon={<ChevronRight />}>
                        {applicationsHierarchy.data.map(application => {
                            return (
                                <TreeItem key={application.name} nodeId="1" label={application.name}/>
                            );
                        })}

                    </TreeView>
                </Grid>
                <Grid>
                    Blah
                </Grid>

            </Grid>

            <Routes>
                <Route path=':applicationId' element={<Application />} />
                <Route path=':applicationId/microservices/:microserviceId' element={<Microservice />} />
                <Route path=':applicationId/microservices/:microserviceId/deployables/:deployableId' element={<Deployable />} />
            </Routes>
        </>
    );
};
