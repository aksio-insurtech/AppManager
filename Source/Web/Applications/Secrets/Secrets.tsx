// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/applications-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { SetSecretDialog, AddSecretDialogOutput } from './SetSecretDialog';
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
        <ValueEditorFor<AddSecretDialogOutput, Secret, Secret>
            addTitle="Add Secret"
            columns={columns}
            data={props.secrets}
            modalContent={SetSecretDialog}
            getRowId={(secret) => secret.key}
            onEditItem={(secret) => secret}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    props.onSecretSet(output as Secret, context);
                }
            }} />
    );
};
