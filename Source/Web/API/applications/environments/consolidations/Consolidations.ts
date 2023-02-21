/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationEnvironmentConsolidation } from './ApplicationEnvironmentConsolidation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidations');

export interface ConsolidationsArguments {
    applicationId: string;
    environmentId: string;
}
export class Consolidations extends ObservableQueryFor<ApplicationEnvironmentConsolidation[], ConsolidationsArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationEnvironmentConsolidation[] = [];

    constructor() {
        super(ApplicationEnvironmentConsolidation, true);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    static use(args?: ConsolidationsArguments): [QueryResultWithState<ApplicationEnvironmentConsolidation[]>] {
        return useObservableQuery<ApplicationEnvironmentConsolidation[], Consolidations, ConsolidationsArguments>(Consolidations, args);
    }
}
