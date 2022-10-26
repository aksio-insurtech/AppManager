// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Content, ModalButtons, ModalClosed, useModal } from '@aksio/cratis-mui';
import { Box, Button, Toolbar } from '@mui/material';
import { DataGrid, GridCallbackDetails, GridColDef, GridRowId, GridRowIdGetter, GridRowsProp, GridSelectionModel, GridValidRowModel } from '@mui/x-data-grid';
import * as icons from '@mui/icons-material';

export type SelectionChanged<TModel> = (rows: TModel[]) => void;
export type RefreshClicked = () => void;

export interface ValueEditorProps<TOutput, TModel extends GridValidRowModel = GridValidRowModel, TInput={}> {
    input?: TInput;
    addTitle: string;
    columns: GridColDef[];
    data: TModel[];
    modalContent: Content<TInput, TOutput>;
    modalClosed: ModalClosed<TOutput>;
    getRowId: GridRowIdGetter<TModel>;
    onSelectionChanged?: SelectionChanged<TModel>;
    onRefresh?: RefreshClicked;
}

export const ValueEditorFor = <TOutput, TModel extends GridValidRowModel = GridValidRowModel, TInput={}>(props: ValueEditorProps<TOutput, TModel, TInput>) => {
    const [showAddDialog] = useModal<TInput, TOutput>(
        props.addTitle,
        ModalButtons.OkCancel,
        props.modalContent,
        props.modalClosed);

    const handleSelectionChanged = (selectionModel: GridSelectionModel, details: GridCallbackDetails) => {
        const selectedItems = selectionModel.map((id => props.data.find(item => props.getRowId(item) == id))) as TModel[];
        props.onSelectionChanged?.(selectedItems);
    };

    return (
        <Box sx={{ height: 400, width: '100%', padding: '24px' }}>
            <Toolbar>
                <Button startIcon={<icons.Add />} onClick={() => showAddDialog(props.input)}>{props.addTitle}</Button>
                {props.onRefresh && <Button startIcon={<icons.Refresh />} onClick={props.onRefresh}/>}
            </Toolbar>

            <DataGrid
                hideFooterPagination={true}
                filterMode="client"
                columns={props.columns}
                sortingMode="client"
                getRowId={props.getRowId}
                onSelectionModelChange={handleSelectionChanged}
                rows={props.data as GridRowsProp<any>} />
        </Box>
    );
};
