// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult, useModal, ModalButtons } from '@aksio/applications-mui';
import { GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { TenantsForEnvironment } from 'API/applications/environments/tenants/TenantsForEnvironment';
import { AddTenantDialog, AddTenantDialogOutput } from './AddTenant';
import { Guid } from '@aksio/fundamentals';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
import { AddTenant } from 'API/applications/environments/tenants/AddTenant';
import { ValueEditorFor } from 'Components';
import { useRouteParams } from '../RouteParams';
import { useState } from 'react';
import { Button } from '@mui/material';
import * as icons from '@mui/icons-material';
import { AssociateDomainDialog } from './AssociateCustomDomainDialog';
import { AssociateDomainWithTenant } from 'API/applications/environments/tenants/AssociateDomainWithTenant';

const columns: GridColDef[] = [
    { field: 'id', headerName: 'Id', width: 350, valueGetter: (params: GridValueGetterParams<any, Tenant>) => params.row.id.tenantId },
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'domain', headerName: 'Domain', width: 250 },
    { field: 'onBehalfOf', headerName: 'OnBehalfOf', width: 250 }
];





export const Tenants = () => {
    const { applicationId, environmentId } = useRouteParams();
    const [tenants] = TenantsForEnvironment.use({ environmentId: environmentId! });
    const [selectedTenant, setSelectedTenant] = useState<Tenant | undefined>();
    const [showAssociateDomainDialog] = useModal(
        `Associate domain with "${selectedTenant?.name}"`,
        ModalButtons.OkCancel,
        AssociateDomainDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new AssociateDomainWithTenant();
                command.applicationId = applicationId;
                command.environmentId = environmentId!;
                command.tenantId = selectedTenant!.id.tenantId;
                command.domain = output!.domain;
                command.certificateId = output!.certificateId;
                await command.execute();
            }
        }
    );


    const toolbar = selectedTenant == undefined ? <></> :
        <>
            <Button startIcon={<icons.Domain />} onClick={() => showAssociateDomainDialog({ environmentId: environmentId! })}>Associate domain</Button>
            <Button startIcon={<icons.ArrowForward />}>Set OnBehalfOf</Button>
        </>;

    return (
        <ValueEditorFor<AddTenantDialogOutput, Tenant>
            addTitle="Add tenant"
            columns={columns}
            data={tenants.data}
            modalContent={AddTenantDialog}
            getRowId={(tenant) => tenant.id.tenantId}
            onSelectionChanged={(rows) => {
                if (rows.length == 1) {
                    setSelectedTenant(rows[0]);
                } else {
                    setSelectedTenant(undefined);
                }
            }}
            toolbarContent={toolbar}
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
