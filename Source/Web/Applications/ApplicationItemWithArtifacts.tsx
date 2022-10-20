// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Divider, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import { ApplicationItem } from './ApplicationItem';
import * as icons from '@mui/icons-material';
import { ApplicationArtifactListItem } from './ApplicationArtifactListItem';


export interface ApplicationItemWithArtifactsProps {
    application: ApplicationHierarchyForListing;
    environmentId?: string;
}

export const ApplicationItemWithArtifacts = (props: ApplicationItemWithArtifactsProps) => {
    const environment = props.application.environments.find(_ => _.environmentId === props.environmentId);

    return (
        <>
            <ApplicationItem application={props.application} environmentId={props.environmentId} actions />
            <Divider />
            {environment &&
                environment.ingresses.map(ingress => {
                    return (
                        <ApplicationArtifactListItem
                            key={ingress.ingressId}
                            icon={<icons.Input />}
                            title={ingress.name} />
                    );
                })
            }

            {/* {props.application.microservices?.map(microservice => {
                return (
                    <ApplicationArtifactListItem
                        key={microservice.microserviceId}
                        icon={<icons.Cabin />}
                        title={microservice.name} />
                );
            })} */}
        </>
    );
};
