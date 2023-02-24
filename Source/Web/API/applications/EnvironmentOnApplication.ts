/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { field } from '@aksio/cratis-fundamentals';

import { ApplicationEnvironmentDeploymentStatus } from './ApplicationEnvironmentDeploymentStatus';
import { TenantInEnvironment } from './TenantInEnvironment';
import { IngressInEnvironment } from './IngressInEnvironment';
import { MicroserviceInEnvironment } from './MicroserviceInEnvironment';

export class EnvironmentOnApplication {

    @field(String)
    environmentId!: string;

    @field(String)
    name!: string;

    @field(Number)
    status!: ApplicationEnvironmentDeploymentStatus;

    @field(Date)
    lastUpdated!: Date;

    @field(Date)
    lastDeploymentStarted!: Date;

    @field(Date)
    lastDeployment!: Date;

    @field(TenantInEnvironment, true)
    tenants!: TenantInEnvironment[];

    @field(IngressInEnvironment, true)
    ingresses!: IngressInEnvironment[];

    @field(MicroserviceInEnvironment, true)
    microservices!: MicroserviceInEnvironment[];

    @field(Boolean)
    hasChanges!: boolean;
}
