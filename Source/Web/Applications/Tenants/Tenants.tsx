// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { TenantsForEnvironment } from 'API/applications/environments/tenants/TenantsForEnvironment';
import { useParams } from 'react-router-dom';
import { AddTenantDialog, AddTenantDialogOutput } from './AddTenant';
import { Guid } from '@aksio/cratis-fundamentals';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
import { AddTenant } from 'API/applications/environments/tenants/AddTenant';
import { ValueEditorFor } from 'Components';

const columns: GridColDef[] = [
    { field: 'id', headerName: 'Id', width: 350 },
    { field: 'name', headerName: 'Name', width: 250 }
];

export const Tenants = () => {
    const { applicationId, environmentId } = useParams();
    const [tenants] = TenantsForEnvironment.use({ environmentId: environmentId! });

    return (
        <ValueEditorFor<AddTenantDialogOutput, Tenant>
            addTitle="Add tenant"
            columns={columns}
            data={tenants.data}
            modalContent={AddTenantDialog}
            getRowId={(tenant) => tenant.id}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    const command = new AddTenant();
                    command.applicationId = applicationId!;
                    command.environmentId = environmentId!;
                    command.tenantId = Guid.create().toString();
                    command.name = output!.name;
                    await command.execute();
                }
            }} />
    );
};
