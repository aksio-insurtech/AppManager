/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { LogMessage } from './LogMessage';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidations/{{consolidationId}}');

export interface ConsolidationArguments {
    applicationId: string;
    environmentId: string;
    consolidationId: string;
}
export class Consolidation extends ObservableQueryFor<LogMessage, ConsolidationArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidations/{{consolidationId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: LogMessage = {} as any;

    constructor() {
        super(LogMessage, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'consolidationId',
        ];
    }

    static use(args?: ConsolidationArguments): [QueryResultWithState<LogMessage>] {
        return useObservableQuery<LogMessage, Consolidation, ConsolidationArguments>(Consolidation, args);
    }
}
