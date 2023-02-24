// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ApplicationEnvironmentDeploymentStatus } from 'API/applications/ApplicationEnvironmentDeploymentStatus';
import { EnvironmentOnApplication } from 'API/applications/EnvironmentOnApplication';
import { ListItemActionButton } from './ListItemActionButton';
import * as icons from '@mui/icons-material';

export type DeployClicked = () => void;

export interface DeploymentButtonProps {
    environment: EnvironmentOnApplication;
    consolidateClicked: DeployClicked;
}

export const DeploymentButton = (props: DeploymentButtonProps) => {
    let element: React.ReactNode;

    switch (props.environment?.status) {
        case ApplicationEnvironmentDeploymentStatus.inProgress:
            element = <ListItemActionButton title="Deploying" icon={<icons.Sync />} />;
            break;

        case ApplicationEnvironmentDeploymentStatus.none:
        case ApplicationEnvironmentDeploymentStatus.completed:
            if (props.environment?.lastUpdated.getTime() === props.environment?.lastDeployment.getTime()) {
                element = <ListItemActionButton title="Run automation" icon={<icons.PlayCircle />} onClick={() => props.consolidateClicked()} />;
            } else {
                element = <ListItemActionButton title="Deploy Changes" icon={<icons.Upgrade />} onClick={() => props.consolidateClicked()} />;
            }
            break;

        case ApplicationEnvironmentDeploymentStatus.failed:
            element = <ListItemActionButton title="Failed" icon={<icons.SyncProblem />} />;
            break;
    }

    return (
        <>
            {element}
        </>
    );
};
