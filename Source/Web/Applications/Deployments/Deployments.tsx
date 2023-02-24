// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Stack, TextareaAutosize, useTheme } from '@mui/material';
import { DataGrid, GridCallbackDetails, GridColDef, GridRowsProp, GridSelectionModel } from '@mui/x-data-grid';

import { useRef, useState } from 'react';
import { useRouteParams } from '../RouteParams';
import { Deployments as DeploymentsQuery } from 'API/applications/environments/deployments/Deployments';
import { ApplicationEnvironmentDeploymentStatus } from 'API/applications/environments/deployments/ApplicationEnvironmentDeploymentStatus';
import { ApplicationEnvironmentDeployment } from 'API/applications/environments/deployments/ApplicationEnvironmentDeployment';
import { Deployment } from 'API/applications/environments/deployments/Deployment';

const statuses = {
    [ApplicationEnvironmentDeploymentStatus.completed]: 'Completed',
    [ApplicationEnvironmentDeploymentStatus.failed]: 'Failed',
    [ApplicationEnvironmentDeploymentStatus.inProgress]: 'In Progress',
    [ApplicationEnvironmentDeploymentStatus.none]: 'Unknown'
};

const columns: GridColDef[] = [
    {
        field: 'status', headerName: 'Status', width: 250,
        valueFormatter: (params) => statuses[params.value as ApplicationEnvironmentDeploymentStatus]
    },
    { field: 'started', headerName: 'Started', width: 250 },
    { field: 'completedOrFailed', headerName: 'Completed', width: 250 },
];

export const Deployments = () => {
    const { applicationId, environmentId } = useRouteParams();
    const [deployments] = DeploymentsQuery.use({ applicationId, environmentId: environmentId! });
    const [selectedDeployment, setSelectedDeployment] = useState<ApplicationEnvironmentDeployment | undefined>(undefined);
    const [deployment] = Deployment.use({ applicationId, environmentId: environmentId!, deploymentId: selectedDeployment?.id.deploymentId || undefined! });
    const textArea = useRef<HTMLTextAreaElement>(null);
    const theme = useTheme();

    const handleSelectionChanged = (selectionModel: GridSelectionModel, details: GridCallbackDetails) => {
        const selectedItems = selectionModel.map(id => deployments.data.find((item) => item.id.deploymentId === id));
        setSelectedDeployment(selectedItems[0]);
    };

    let log = deployment.data.message || '';
    log = log.replace(/\\n/g, '\n');

    if (textArea.current) {
        textArea.current.scrollTop = textArea.current.scrollHeight;
    }

    return (
        <Stack
            direction="column"
            justifyContent="flex-start"
            height={'100%'}
            spacing={5}>

            <Box sx={{ height: 600, width: '100%', padding: '24px' }}>
                <DataGrid
                    hideFooterPagination={true}
                    filterMode="client"
                    columns={columns}
                    sortingMode="client"
                    getRowId={(row: ApplicationEnvironmentDeployment) => row.id.deploymentId}
                    onSelectionModelChange={handleSelectionChanged}
                    rows={deployments.data as GridRowsProp<any>} />

            </Box>
            <Box sx={{ height: '100%', width: '100%', paddingLeft: '24px', paddingRight: '24px', paddingBottom: '100px' }}>
                <textarea style={{
                    width: '100%',
                    height: '100%',
                    whiteSpace: 'pre-wrap',
                    backgroundColor: theme.palette.background.default,
                    color: theme.palette.primary.main,
                    resize: 'none'
                }} readOnly value={log} ref={textArea} />
            </Box>
        </Stack>
    );
};
