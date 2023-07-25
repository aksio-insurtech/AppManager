/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { ApplicationEnvironmentDeployment } from './ApplicationEnvironmentDeployment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/deployments');

export interface DeploymentsArguments {
    applicationId: string;
    environmentId: string;
}
export class Deployments extends ObservableQueryFor<ApplicationEnvironmentDeployment[], DeploymentsArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/deployments';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationEnvironmentDeployment[] = [];

    constructor() {
        super(ApplicationEnvironmentDeployment, true);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    static use(args?: DeploymentsArguments): [QueryResultWithState<ApplicationEnvironmentDeployment[]>] {
        return useObservableQuery<ApplicationEnvironmentDeployment[], Deployments, DeploymentsArguments>(Deployments, args);
    }
}
