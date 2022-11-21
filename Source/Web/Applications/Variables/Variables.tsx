// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { AddVariableDialog, AddVariableDialogOutput } from './AddVariableDialog';
import { useRouteParams, RouteParams } from '../RouteParams';
import { EnvironmentVariable } from 'API/applications/EnvironmentVariable';

const columns: GridColDef[] = [
    { field: 'key', headerName: 'Key', width: 250 },
    { field: 'value', headerName: 'Value', width: 250 }
];

export interface Variable {
    key: string;
    value: string;
}

export type VariableSet = (variable: Variable, context: RouteParams) => void;

export interface VariablesProps {
    onVariableSet: VariableSet;
    variables: EnvironmentVariable[];
}

export const Variables = (props: VariablesProps) => {
    const context = useRouteParams();

    return (
        <ValueEditorFor<AddVariableDialogOutput, EnvironmentVariable>
            addTitle="Add Variable"
            columns={columns}
            data={props.variables}
            modalContent={AddVariableDialog}
            getRowId={(variable) => variable.key}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    props.onVariableSet(output as Variable, context);
                }
            }} />
    );
};
