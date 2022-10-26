// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
import { ValueEditorFor } from 'Components';
import { AddVariableDialog, AddVariableDialogOutput } from './AddVariableDialog';
import { useRouteParams, RouteParams } from '../RouteParams';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'value', headerName: 'Value', width: 250 }
];

export interface Variable {
    key: string;
    value: string;
}

export type VariableSet = (variable: Variable, context: RouteParams) => void;

export interface VariablesProps {
    onVariableSet: VariableSet
}

export const Variables = (props: VariablesProps) => {
    const context = useRouteParams();

    return (
        <ValueEditorFor<AddVariableDialogOutput, Tenant>
            addTitle="Add Variable"
            columns={columns}
            data={[]}
            modalContent={AddVariableDialog}
            getRowId={(tenant) => tenant.id}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    props.onVariableSet(output as Variable, context);
                }
            }} />
    );
};
