using System;
using GC = GoogleCloudClassLibrary.VisualHeader;

namespace GoogleCloudClassLibrary.Intelligence {
    public class VideoIntelligence {
        public GC.Operation AnnotateVideoWithLabelDetection(String videoUri, String videoData, GC.VideoContext context,
            String outputUri = "", String cloudRegionId = "") {
                return null;
        }

        public GC.Operation AnnotateVideoWithShotChangeDetection(String videoUri, String videoData, GC.VideoContext context,
            String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public GC.Operation AnnotateVideoWithExplicitContentDetection(String videoUri, String videoData, 
            GC.VideoContext context, String outputUri = "", String cloudRegionId = "") {
            return null;
        }

        public GC.Operation AnnotateVideoWithMultipleDetections(String videoUri, String videoData, GC.VideoContext context,
            String outputUri = "", String cloudRegionId = "", Boolean labelDetection = false, 
            Boolean shotChangeDetection = false, Boolean explicitContentDetection = false) {
            return null;
        }
    }
}