// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ApplicationEnvironmentConsolidationStatus } from 'API/applications/ApplicationEnvironmentConsolidationStatus';
import { EnvironmentOnApplication } from 'API/applications/EnvironmentOnApplication';
import { ListItemActionButton } from './ListItemActionButton';
import * as icons from '@mui/icons-material';

export type ConsolidateClicked = () => void;

export interface ConsolidationButtonProps {
    environment: EnvironmentOnApplication;
    consolidateClicked: ConsolidateClicked;
}

export const ConsolidationButton = (props: ConsolidationButtonProps) => {
    let element: React.ReactNode;

    switch (props.environment?.status) {
        case ApplicationEnvironmentConsolidationStatus.inProgress:
            element = <ListItemActionButton title="Consolidating" icon={<icons.Sync />} />;
            break;

        case ApplicationEnvironmentConsolidationStatus.none:
        case ApplicationEnvironmentConsolidationStatus.completed:
            element = <ListItemActionButton title="Consolidate Changes" icon={<icons.Upgrade />} onClick={() => props.consolidateClicked()} />;
            break;

        case ApplicationEnvironmentConsolidationStatus.failed:
            element = <ListItemActionButton title="Consolidating" icon={<icons.SyncProblem />} />;
            break;
    }

    return (
        <>
            {element}
        </>
    );
};
