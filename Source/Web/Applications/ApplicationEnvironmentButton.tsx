// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import * as icons from '@mui/icons-material';
import { MenuItem } from '@mui/material';
import { EnvironmentOnApplication } from 'API/applications/EnvironmentOnApplication';
import { useNavigate } from 'react-router-dom';
import { ActionButtonWithMenu } from './ActionButtonWithMenu';

export interface ApplicationEnvironmentButtonProps {
    application: ApplicationHierarchyForListing
}

export const ApplicationEnvironmentButton = (props: ApplicationEnvironmentButtonProps) => {
    const navigate = useNavigate();

    const handleEnvironmentSelected = (environment: EnvironmentOnApplication) => {
        navigate(`/applications/${props.application.id}/environments/${environment.environmentId}`);
    };

    return (
        <ActionButtonWithMenu title="Select environment" icon={<icons.AltRoute/>}>
                {props.application.environments.map(environment => {
                    return (
                        <MenuItem
                            key={environment.environmentId}
                            onClick={() => handleEnvironmentSelected(environment)}>{environment.name}</MenuItem>
                    );
                })}
        </ActionButtonWithMenu>
    );
};

