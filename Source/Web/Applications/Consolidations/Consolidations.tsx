// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Stack, TextareaAutosize } from '@mui/material';
import { DataGrid, GridCallbackDetails, GridColDef, GridRowsProp, GridSelectionModel } from '@mui/x-data-grid';

import { useRef, useState } from 'react';
import { useRouteParams } from '../RouteParams';
import { Consolidations as ConsolidationsQuery } from 'API/applications/environments/consolidations/Consolidations';
import { ApplicationEnvironmentConsolidationStatus } from 'API/applications/environments/consolidations/ApplicationEnvironmentConsolidationStatus';
import { ApplicationEnvironmentConsolidation } from 'API/applications/environments/consolidations/ApplicationEnvironmentConsolidation';
import { Consolidation } from 'API/applications/environments/consolidations/Consolidation';

const statuses = {
    [ApplicationEnvironmentConsolidationStatus.completed]: 'Completed',
    [ApplicationEnvironmentConsolidationStatus.failed]: 'Failed',
    [ApplicationEnvironmentConsolidationStatus.inProgress]: 'In Progress',
    [ApplicationEnvironmentConsolidationStatus.none]: 'Unknown'
}


const columns: GridColDef[] = [
    {
        field: 'status', headerName: 'Status', width: 250,
        valueFormatter: (params) => statuses[params.value as ApplicationEnvironmentConsolidationStatus]
    },
    { field: 'started', headerName: 'Started', width: 250 },
    { field: 'completedOrFailed', headerName: 'Completed', width: 250 },
];

export const Consolidations = () => {
    const { applicationId, environmentId } = useRouteParams();
    const [consolidations] = ConsolidationsQuery.use({ applicationId, environmentId: environmentId! });
    const [selectedConsolidation, setSelectedConsolidation] = useState<ApplicationEnvironmentConsolidation | undefined>(undefined);
    const [consolidation] = Consolidation.use({ applicationId, environmentId: environmentId!, consolidationId: selectedConsolidation?.id.consolidationId || undefined! });
    const textArea = useRef<HTMLTextAreaElement>(null);

    const handleSelectionChanged = (selectionModel: GridSelectionModel, details: GridCallbackDetails) => {
        const selectedItems = selectionModel.map(id => consolidations.data.find((item) => item.id.consolidationId === id));
        setSelectedConsolidation(selectedItems[0]);
    };

    let log = consolidation.data.message || '';
    log = log.replace(/\\n/g, '\n');

    if (textArea.current) {
        textArea.current.scrollTop = textArea.current.scrollHeight;
    }

    return (
        <Stack
            direction="column"
            justifyContent="flex-start"
            height={'100%'}
            spacing={5}>

            <Box sx={{ height: 600, width: '100%', padding: '24px' }}>
                <DataGrid
                    hideFooterPagination={true}
                    filterMode="client"
                    columns={columns}
                    sortingMode="client"
                    getRowId={(row: ApplicationEnvironmentConsolidation) => row.id.consolidationId}
                    onSelectionModelChange={handleSelectionChanged}
                    rows={consolidations.data as GridRowsProp<any>} />

            </Box>
            <Box sx={{ height: '100%', width: '100%', paddingLeft: '24px', paddingRight: '24px' }}>
                <textarea style={{ width: '100%', height: '100%', whiteSpace: 'pre-wrap' }} readOnly value={log} ref={textArea} />
            </Box>
        </Stack>
    );
};
