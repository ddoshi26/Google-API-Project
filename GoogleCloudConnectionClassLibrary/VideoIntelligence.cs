using System;
using GC = GoogleCloudClassLibrary.VisualHeader;

namespace GoogleCloudClassLibrary.VisualIntelligence {
    public class VideoIntelligence {
        
        // TODO: Change all return types to Operation class

        public String AnnotateVideoWithLabelDetection(String videoUri, String videoData, GC.VideoContext context,
            String outputUri = "", String cloudRegionId = "") {
                return null;
        }

        public String AnnotateVideoWithShotChangeDetection(String videoUri, String videoData, GC.VideoContext context,
            String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public String AnnotateVideoWithExplicitContentDetection(String videoUri, String videoData, 
            GC.VideoContext context, String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public String AnnotateVideoWithMultipleDetections(String videoUri, String videoData, GC.VideoContext context,
            String outputUri = "", String cloudRegionId = "", Boolean labelDetection = false, 
            Boolean shotChangeDetection = false, Boolean explicitContentDetection = false) {
            return null;
        }
    }
}