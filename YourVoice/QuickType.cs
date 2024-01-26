//This is an automatically generated file from QuickType.
//If you are unaware of what QuickType is, it allows you to
//Quickly generate JSON application code from an example JSON
//File. It is extremely useful, free, and awesome.
//
// https://www.quicktype.io/
// Github: https://github.com/glideapps/quicktype
//

namespace QuickType {

    using System;
    using System.Collections.Generic;

    using J = Newtonsoft.Json.JsonPropertyAttribute;
    using N = Newtonsoft.Json.NullValueHandling;

    public partial class FineTuning {
        [J( "fine_tuning_requested" )] public bool FineTuningRequested { get; set; }

        [J( "finetuning_state" )] public string FinetuningState { get; set; }

        [J( "is_allowed_to_fine_tune" )] public bool IsAllowedToFineTune { get; set; }

        [J( "language" )] public object Language { get; set; }

        [J( "manual_verification" )] public object ManualVerification { get; set; }

        [J( "manual_verification_requested" )] public bool ManualVerificationRequested { get; set; }

        [J( "slice_ids" )] public object SliceIds { get; set; }

        [J( "verification_attempts" )] public object VerificationAttempts { get; set; }

        [J( "verification_attempts_count" )] public long VerificationAttemptsCount { get; set; }

        [J( "verification_failures" )] public List<object> VerificationFailures { get; set; }
    }

    public partial class Labels {
        [J( "accent", NullValueHandling = N.Ignore )] public string Accent { get; set; }

        [J( "age" )] public string Age { get; set; }

        [J( "description", NullValueHandling = N.Ignore )] public string Description { get; set; }

        [J( "gender" )] public string Gender { get; set; }

        [J( "description ", NullValueHandling = N.Ignore )] public string LabelsDescription { get; set; }

        [J( "usecase", NullValueHandling = N.Ignore )] public string Usecase { get; set; }

        [J( "use case", NullValueHandling = N.Ignore )] public string UseCase { get; set; }
    }

    public partial class Root {
        [J( "voices" )] public List<Voice> Voices { get; set; }
    }

    public partial class Voice {
        [J( "available_for_tiers" )] public List<object> AvailableForTiers { get; set; }

        [J( "category" )] public string Category { get; set; }

        [J( "description" )] public object Description { get; set; }

        [J( "fine_tuning" )] public FineTuning FineTuning { get; set; }

        [J( "high_quality_base_model_ids" )] public List<string> HighQualityBaseModelIds { get; set; }

        [J( "labels" )] public Labels Labels { get; set; }

        [J( "name" )] public string Name { get; set; }

        [J( "preview_url" )] public Uri PreviewUrl { get; set; }

        [J( "samples" )] public object Samples { get; set; }

        [J( "settings" )] public object Settings { get; set; }

        [J( "sharing" )] public object Sharing { get; set; }

        [J( "voice_id" )] public string VoiceId { get; set; }
    }
}
