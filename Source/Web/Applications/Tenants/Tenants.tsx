// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-mui';
import { Box, Button, Toolbar } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { TenantsForEnvironment } from 'API/applications/environments/tenants/TenantsForEnvironment';
import { useParams } from 'react-router-dom';
import { AddTenantDialog } from './AddTenant';
import * as icons from '@mui/icons-material';
import { Guid } from '@aksio/cratis-fundamentals';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
import { AddTenant } from 'API/applications/environments/tenants/AddTenant';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 }
];


export const Tenants = () => {
    const { applicationId, environmentId } = useParams();
    const [tenants] = TenantsForEnvironment.use({ environmentId: environmentId! });

    const [showAddTenantDialog] = useModal(
        'Add tenant',
        ModalButtons.OkCancel,
        AddTenantDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new AddTenant();
                command.applicationId = applicationId!;
                command.environmentId = environmentId!;
                command.tenantId = Guid.create().toString();
                command.name = output!.name;
                await command.execute();
            }
        });

    return (
        <Box sx={{ height: 400, width: '100%', padding: '24px' }}>
            <Toolbar>
                <Button startIcon={<icons.Add />} onClick={showAddTenantDialog}>Add Tenant</Button>
            </Toolbar>

            <DataGrid
                hideFooterPagination={true}
                filterMode="client"
                columns={columns}
                sortingMode="client"
                getRowId={(row: Tenant) => {
                    return row.id;
                }}

                rows={tenants.data} />
        </Box>
    );
};
