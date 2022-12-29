// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { AddSecretDialog, AddSecretDialogOutput } from './AddSecretDialog';
import { RouteParams, useRouteParams } from '../RouteParams';
import { Secret } from 'API/applications/Secret';

const columns: GridColDef[] = [
    { field: 'key', headerName: 'Key', width: 250 },
    { field: 'value', headerName: 'Value', width: 250 }
];

export type SecretSet = (secret: Secret, context: RouteParams) => void;

export interface SecretsProps {
    onSecretSet: SecretSet;
    secrets: Secret[];
}

export const Secrets = (props: SecretsProps) => {
    const context = useRouteParams();

    return (
        <ValueEditorFor<AddSecretDialogOutput, Secret>
            addTitle="Add Secret"
            columns={columns}
            data={props.secrets}
            modalContent={AddSecretDialog}
            getRowId={(secret) => secret.key}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    props.onSecretSet(output as Secret, context);
                }
            }} />
    );
};
