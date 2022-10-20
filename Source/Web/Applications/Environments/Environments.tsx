// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Button, IconButton, Toolbar } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import * as icons from '@mui/icons-material';
import { useModal, ModalButtons, ModalResult } from '@aksio/cratis-mui';
import { AddEnvironmentDialog } from './AddEnvironmentDialog';
import { EnvironmentsForApplication } from 'API/applications/environments/EnvironmentsForApplication';
import { ApplicationEnvironment } from 'API/applications/environments/ApplicationEnvironment';
import { useParams } from 'react-router-dom';
import { CreateEnvironment } from '../../API/applications/environments/CreateEnvironment';
import { Guid } from '@aksio/cratis-fundamentals';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'displayName', headerName: 'Display Name', width: 250 },
    { field: 'shortName', headerName: 'Short Display Name', width: 150 }
];

export const Environments = () => {
    const { applicationId } = useParams();
    const [environments] = EnvironmentsForApplication.use({ applicationId: applicationId! });

    const [showAddEnvironmentDialog] = useModal(
        'Add environment',
        ModalButtons.OkCancel,
        AddEnvironmentDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new CreateEnvironment();
                command.applicationId = applicationId!;
                command.environmentId = Guid.create().toString();
                command.name = output!.name;
                command.displayName = output!.displayName;
                command.shortName = output!.shortName;
                await command.execute();
            }
        });

    return (
        <Box sx={{ height: 400, width: '100%' }}>
            <Toolbar>
                <Button startIcon={<icons.Add />} onClick={showAddEnvironmentDialog}>Add Environment</Button>
            </Toolbar>

            <DataGrid
                hideFooterPagination={true}
                filterMode="client"
                columns={columns}
                sortingMode="client"
                getRowId={(row: ApplicationEnvironment) => {
                    return row.id;
                }}

                rows={environments.data} />
        </Box>
    );
};
