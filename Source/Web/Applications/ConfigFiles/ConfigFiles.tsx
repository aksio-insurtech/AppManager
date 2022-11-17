// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
import { ValueEditorFor } from 'Components';
import { AddConfigFileDialog, AddConfigFileDialogOutput } from './AddConfigFIleDialog';
import { useRouteParams } from '../RouteParams';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 }
];

export const ConfigFiles = () => {
    const { applicationId, environmentId } = useRouteParams();

    return (
        <ValueEditorFor<AddConfigFileDialogOutput, Tenant>
            addTitle="Add config file"
            columns={columns}
            data={[]}
            modalContent={AddConfigFileDialog}
            getRowId={(tenant) => tenant.id.tenantId}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                }
            }} />
    );
};
