// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { useParams } from 'react-router-dom';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
import { ValueEditorFor } from 'Components';
import { AddSecretDialog, AddSecretDialogOutput } from './AddSecretDialog';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'value', headerName: 'Value', width: 250 }
];

export const Secrets = () => {
    const { applicationId, environmentId } = useParams();

    return (
        <ValueEditorFor<AddSecretDialogOutput, Tenant>
            addTitle="Add Secret"
            columns={columns}
            data={[]}
            modalContent={AddSecretDialog}
            getRowId={(tenant) => tenant.id}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                }
            }} />
    );
};
