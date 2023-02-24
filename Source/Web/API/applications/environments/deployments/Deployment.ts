/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { LogMessage } from './LogMessage';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/deployments/{{deploymentId}}');

export interface DeploymentArguments {
    applicationId: string;
    environmentId: string;
    deploymentId: string;
}
export class Deployment extends ObservableQueryFor<LogMessage, DeploymentArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/deployments/{{deploymentId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: LogMessage = {} as any;

    constructor() {
        super(LogMessage, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'deploymentId',
        ];
    }

    static use(args?: DeploymentArguments): [QueryResultWithState<LogMessage>] {
        return useObservableQuery<LogMessage, Deployment, DeploymentArguments>(Deployment, args);
    }
}
