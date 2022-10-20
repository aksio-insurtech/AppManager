// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ListItem, ListItemButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';
import { ListItemActionButton } from './ListItemActionButton';
import * as icons from '@mui/icons-material';
import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import { Create as CreateMicroservice } from 'API/applications/environments/microservices/Create';
import { useNavigate } from 'react-router-dom';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-mui';
import { CreateMicroserviceDialog } from './CreateMicroserviceDialog';
import { Guid } from '@aksio/cratis-fundamentals';
import { ApplicationEnvironmentButton } from './ApplicationEnvironmentButton';
import { ActionButtonWithMenu } from './ActionButtonWithMenu';

export interface ApplicationItemProps {
    application: ApplicationHierarchyForListing;
    environmentId?: string;
    actions?: boolean;
}


export const ApplicationItem = (props: ApplicationItemProps) => {
    const navigate = useNavigate();

    const [showCreateMicroservice] = useModal(
        'Create microservice',
        ModalButtons.OkCancel,
        CreateMicroserviceDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new CreateMicroservice();
                command.applicationId = output!.applicationId;
                command.microserviceId = Guid.create().toString();
                command.name = output!.name;
                await command.execute();
            }
        }
    );

    return (
        <ListItem component="div" disablePadding>
            <ListItemButton sx={{ height: 56 }} onClick={() => {
                navigate(`/applications/${props.application.id}`);
            }}>
                <ListItemIcon>
                    <icons.Home color="primary" />
                </ListItemIcon>
                <ListItemText primaryTypographyProps={{
                    color: 'primary',
                    fontWeight: 'medium',
                    variant: 'body2'
                }}>{props.application.name}</ListItemText>
            </ListItemButton>

            {props.actions &&
                <>
                    {props.application.environments.length > 0 &&
                        <ApplicationEnvironmentButton application={props.application} />
                    }

                    {props.environmentId &&
                        <ActionButtonWithMenu icon={<icons.Add />} title="Add">
                            <MenuItem onClick={() => {
                                showCreateMicroservice({
                                    applicationId: props.application.id
                                });
                            }} > Add Microservice</MenuItem>
                        </ActionButtonWithMenu>

                    }
                    <ListItemActionButton title="Application Settings" icon={<icons.Settings />} arrow />
                </>
            }
        </ListItem >
    );
};
