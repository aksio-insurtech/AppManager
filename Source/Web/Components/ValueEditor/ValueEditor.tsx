// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Content, ModalButtons, ModalClosed, useModal } from '@aksio/cratis-mui';
import { Box, Button, Toolbar } from '@mui/material';
import { DataGrid, GridCallbackDetails, GridColDef, GridRowId, GridRowIdGetter, GridRowsProp, GridSelectionModel, GridValidRowModel, GridCellParams } from '@mui/x-data-grid';
import * as icons from '@mui/icons-material';

export type SelectionChanged<TModel> = (rows: TModel[]) => void;
export type RefreshClicked = () => void;
export type EditItem<TModel, TInput> = (item: TModel) => TInput;

export interface ValueEditorProps<TOutput, TModel extends GridValidRowModel = GridValidRowModel, TInput = {}> {
    input?: TInput;
    addTitle: string;
    columns: GridColDef[];
    data: TModel[];
    modalContent: Content<TInput, TOutput>;
    modalClosed: ModalClosed<TOutput>;
    getRowId: GridRowIdGetter<TModel>;
    onEditItem?: EditItem<TModel, TInput>;
    onSelectionChanged?: SelectionChanged<TModel>;
    onRefresh?: RefreshClicked;
    toolbarContent?: React.ReactNode;
}

export const ValueEditorFor = <TOutput, TModel extends GridValidRowModel = GridValidRowModel, TInput = {}>(props: ValueEditorProps<TOutput, TModel, TInput>) => {
    const [showEditorDialog] = useModal<TInput, TOutput>(
        props.addTitle,
        ModalButtons.OkCancel,
        props.modalContent,
        props.modalClosed);

    const handleSelectionChanged = (selectionModel: GridSelectionModel, details: GridCallbackDetails) => {
        const selectedItems = selectionModel.map((id => props.data.find(item => props.getRowId(item) == id))) as TModel[];
        props.onSelectionChanged?.(selectedItems);
    };

    const handleEditItem = (e: GridCellParams) => {
        if (props.onEditItem) {
            const input = props.onEditItem(e.row);
            showEditorDialog(input);
        }
    };

    return (
        <Box sx={{ height: 400, width: '100%', padding: '24px' }}>
            <Toolbar>
                <Button startIcon={<icons.Add />} onClick={() => showEditorDialog(props.input)}>{props.addTitle}</Button>
                {props.onRefresh && <Button startIcon={<icons.Refresh />} onClick={props.onRefresh} />}
                {props.toolbarContent && props.toolbarContent}
            </Toolbar>

            <DataGrid
                hideFooterPagination={true}
                filterMode="client"
                columns={props.columns}
                sortingMode="client"
                getRowId={props.getRowId}
                onCellDoubleClick={handleEditItem}
                onSelectionModelChange={handleSelectionChanged}
                rows={props.data as GridRowsProp<any>} />
        </Box>
    );
};
