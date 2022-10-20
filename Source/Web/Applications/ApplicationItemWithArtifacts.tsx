// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Divider, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { ApplicationsHierarchyForListing } from 'API/applications/ApplicationsHierarchyForListing';
import { ApplicationItem } from './ApplicationItem';
import * as icons from '@mui/icons-material';
import { ApplicationArtifactListItem } from './ApplicationArtifactListItem';


export interface ApplicationItemWithArtifactsProps {
    application: ApplicationsHierarchyForListing;
}

export const ApplicationItemWithArtifacts = (props: ApplicationItemWithArtifactsProps) => {
    return (
        <>
            <ApplicationItem application={props.application} actions />
            <Divider />
            <ApplicationArtifactListItem
                icon={<icons.Input />}
                title="Ingress" />

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
