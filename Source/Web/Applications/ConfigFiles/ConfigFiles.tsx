// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { AddConfigFileDialog, AddConfigFileDialogOutput } from './AddConfigFIleDialog';
import { useRouteParams, RouteParams } from '../RouteParams';
import { ConfigFile } from 'API/applications/ConfigFile';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 }
];

export type ConfigFileSet = (file: ConfigFile, context: RouteParams) => void;

export interface ConfigFilesProps {
    onConfigFileSet: ConfigFileSet;
    files: ConfigFile[]
}

export const ConfigFiles = (props: ConfigFilesProps) => {
    const context = useRouteParams();

    return (
        <ValueEditorFor<AddConfigFileDialogOutput, ConfigFile>
            addTitle="Add config file"
            columns={columns}
            data={props.files}
            modalContent={AddConfigFileDialog}
            getRowId={(file) => file.name}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    props.onConfigFileSet(output as ConfigFile, context);
                }
            }} />
    );
};
