// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ListItem, ListItemButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';
import * as icons from '@mui/icons-material';
import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import { CreateMicroservice } from 'API/applications/environments/microservices/CreateMicroservice';
import { useNavigate } from 'react-router-dom';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-mui';
import { CreateMicroserviceDialog } from './CreateMicroserviceDialog';
import { Guid } from '@aksio/cratis-fundamentals';
import { ApplicationEnvironmentButton } from './ApplicationEnvironmentButton';
import { ActionButtonWithMenu } from './ActionButtonWithMenu';
import { CreateIngressDialog } from './CreateIngressDialog';
import { CreateIngress } from 'API/applications/environments/ingresses/CreateIngress';
import { ConsolidateApplicationEnvironment } from 'API/applications/environments/ConsolidateApplicationEnvironment';
import { ConsolidationButton } from './ConsolidationButton';

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
                command.environmentId = props.environmentId!;
                command.microserviceId = Guid.create().toString();
                command.name = output!.name;
                await command.execute();
            }
        }
    );

    const [showCreateIngress] = useModal(
        'Create ingress',
        ModalButtons.OkCancel,
        CreateIngressDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new CreateIngress();
                command.applicationId = output!.applicationId;
                command.environmentId = props.environmentId!;
                command.ingressId = Guid.create().toString();
                command.name = output!.name;
                await command.execute();
            }
        }
    );

    const [consolidateApplicationEnvironment] = ConsolidateApplicationEnvironment.use({
        applicationId: props.application.id,
        environmentId: props.environmentId
    });


    const currentEnvironment = props.application.environments?.find(_ => _.environmentId === props.environmentId);

    return (
        <ListItem component="div" disablePadding>
            <ListItemButton sx={{ height: 56 }} onClick={() => {
                let path = `/applications/${props.application.id}`;
                if (props.environmentId) {
                    path += `/environments/${props.environmentId}`;
                }
                navigate(path);
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
                    {props.application.environments?.length > 0 &&
                        <ApplicationEnvironmentButton application={props.application} />
                    }

                    {props.environmentId &&
                        <>
                            <ActionButtonWithMenu icon={<icons.Add />} title="Add">
                                <MenuItem onClick={() => {
                                    showCreateIngress({
                                        applicationId: props.application.id
                                    });
                                }} >Add Ingress</MenuItem>
                                <MenuItem onClick={() => {
                                    showCreateMicroservice({
                                        applicationId: props.application.id
                                    });
                                }} >Add Microservice</MenuItem>
                            </ActionButtonWithMenu>
                            <ConsolidationButton environment={currentEnvironment!} consolidateClicked={() => consolidateApplicationEnvironment.execute()} />
                        </>
                    }
                </>
            }
        </ListItem >
    );
};
