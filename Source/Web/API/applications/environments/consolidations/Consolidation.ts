/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationEnvironmentConsolidation } from './ApplicationEnvironmentConsolidation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidations/{{consolidationId}}');

export interface ConsolidationArguments {
    applicationId: string;
    environmentId: string;
    consolidationId: string;
}
export class Consolidation extends ObservableQueryFor<ApplicationEnvironmentConsolidation, ConsolidationArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidations/{{consolidationId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationEnvironmentConsolidation = {} as any;

    constructor() {
        super(ApplicationEnvironmentConsolidation, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'consolidationId',
        ];
    }

    static use(args?: ConsolidationArguments): [QueryResultWithState<ApplicationEnvironmentConsolidation>] {
        return useObservableQuery<ApplicationEnvironmentConsolidation, Consolidation, ConsolidationArguments>(Consolidation, args);
    }
}
