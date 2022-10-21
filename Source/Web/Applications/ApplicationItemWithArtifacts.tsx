// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Divider, ListItemButton, ListItemIcon, ListItemText, ListSubheader } from '@mui/material';
import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import { ApplicationItem } from './ApplicationItem';
import * as icons from '@mui/icons-material';
import { ApplicationArtifactListItem } from './ApplicationArtifactListItem';
import { useNavigate } from 'react-router-dom';
import { MicroserviceInEnvironment } from 'API/applications/MicroserviceInEnvironment';
import { IngressInEnvironment } from 'API/applications/IngressInEnvironment';


export interface ApplicationItemWithArtifactsProps {
    application: ApplicationHierarchyForListing;
    environmentId?: string;
}

export const ApplicationItemWithArtifacts = (props: ApplicationItemWithArtifactsProps) => {
    const navigate = useNavigate();
    const environment = props.application.environments.find(_ => _.environmentId === props.environmentId);

    const handleIngressClick = (ingress: IngressInEnvironment) => {
        navigate(`/applications/${props.application.id}/environments/${props.environmentId}/ingresses/${ingress.ingressId}`);
    };

    const handleTenantsClick = () => {
        navigate(`/applications/${props.application.id}/environments/${props.environmentId}/tenants`);
    };

    const handleMicroserviceClick = (microservice: MicroserviceInEnvironment) => {
        navigate(`/applications/${props.application.id}/environments/${props.environmentId}/microservices/${microservice.microserviceId}`);
    };

    return (
        <>
            <ApplicationItem application={props.application} environmentId={props.environmentId} actions />
            <Divider />
            {environment &&
                <>
                    <ListSubheader>{environment.name}</ListSubheader>
                    <ApplicationArtifactListItem
                        icon={<icons.Apartment />}
                        title="Tenants"
                        onClick={handleTenantsClick} />

                    {environment.ingresses?.map(ingress => {
                        return (
                            <ApplicationArtifactListItem
                                key={ingress.ingressId}
                                icon={<icons.Input />}
                                title={ingress.name}
                                onClick={() => handleIngressClick(ingress)} />
                        );
                    })}

                    {environment.microservices?.map(microservice => {
                        return (
                            <ApplicationArtifactListItem
                                key={microservice.microserviceId}
                                icon={<icons.Cabin />}
                                title={microservice.name}
                                onClick={() => handleMicroserviceClick(microservice)} />
                        );
                    })}
                </>
            }
        </>
    );
};
