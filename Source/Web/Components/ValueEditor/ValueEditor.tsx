// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Content, ModalButtons, ModalClosed, useModal } from '@aksio/cratis-mui';
import { Box, Button, Toolbar } from '@mui/material';
import { DataGrid, GridColDef, GridRowId, GridRowIdGetter, GridRowsProp, GridValidRowModel } from '@mui/x-data-grid';
import * as icons from '@mui/icons-material';

export interface ValueEditorProps<TOutput, TModel extends GridValidRowModel = GridValidRowModel> {
    addTitle: string;
    columns: GridColDef[];
    data: TModel[];
    modalContent: Content<{}, TOutput>;
    modalClosed: ModalClosed<TOutput>;
    getRowId: GridRowIdGetter<TModel>;
}

export const ValueEditorFor = <TOutput, TModel extends GridValidRowModel = GridValidRowModel>(props: ValueEditorProps<TOutput, TModel>) => {
    const [showAddDialog] = useModal(
        props.addTitle,
        ModalButtons.OkCancel,
        props.modalContent,
        props.modalClosed);

    return (
        <Box sx={{ height: 400, width: '100%', padding: '24px' }}>
            <Toolbar>
                <Button startIcon={<icons.Add />} onClick={showAddDialog}>Add Tenant</Button>
            </Toolbar>

            <DataGrid
                hideFooterPagination={true}
                filterMode="client"
                columns={props.columns}
                sortingMode="client"
                getRowId={props.getRowId}
                rows={props.data as GridRowsProp<any>} />
        </Box>
    );
};
