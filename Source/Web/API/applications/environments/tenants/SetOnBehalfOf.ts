/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/tenants/{{tenantId}}/custom-domain');

export interface ISetOnBehalfOf {
    applicationId?: string;
    environmentId?: string;
    tenantId?: string;
    onBehalfOf?: string;
}

export class SetOnBehalfOfValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
        tenantId: new Validator(),
        onBehalfOf: new Validator(),
    };
}

export class SetOnBehalfOf extends Command<ISetOnBehalfOf> implements ISetOnBehalfOf {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/tenants/{{tenantId}}/custom-domain';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetOnBehalfOfValidator();

    private _applicationId!: string;
    private _environmentId!: string;
    private _tenantId!: string;
    private _onBehalfOf!: string;

    constructor() {
        super(Object, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'tenantId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
            'tenantId',
            'onBehalfOf',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }
    get tenantId(): string {
        return this._tenantId;
    }

    set tenantId(value: string) {
        this._tenantId = value;
        this.propertyChanged('tenantId');
    }
    get onBehalfOf(): string {
        return this._onBehalfOf;
    }

    set onBehalfOf(value: string) {
        this._onBehalfOf = value;
        this.propertyChanged('onBehalfOf');
    }

    static use(initialValues?: ISetOnBehalfOf): [SetOnBehalfOf, SetCommandValues<ISetOnBehalfOf>, ClearCommandValues] {
        return useCommand<SetOnBehalfOf, ISetOnBehalfOf>(SetOnBehalfOf, initialValues);
    }
}
