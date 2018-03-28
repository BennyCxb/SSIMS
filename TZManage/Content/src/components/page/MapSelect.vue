<template>
  <el-dialog title="选择位置" :visible.sync="mapShow" size="tiny" :before-close="handleClose" width="100%">
    <div class="amap-page-container">
      <el-amap-search-box class="search-box" :search-option="searchOption"
                          :on-search-result="onSearchResult"></el-amap-search-box>
      <el-amap
        vid="amapDemo"
        :center="center"
        :zoom="zoom"
        :plugin="plugin"
        class="amap-demo"
        :events="events">
        <!--<el-amap-marker :position="position"></el-amap-marker>-->
        <el-amap-marker v-for="(marker, index) in markers" :position="marker.position" :key="index" :vid="index"></el-amap-marker>
      </el-amap>
    </div>
    <div class="toolbar">
      <p>Position: [{{ lng }}, {{ lat }}]</p>
      <p>Address: {{ address }}</p>
    </div>
    <span slot="footer" class="dialog-footer">
        <el-button @click="handleClose">取 消</el-button>
        <el-button type="primary" @click="done">确 定</el-button>
      </span>
  </el-dialog>
</template>

<style>
  #amapDemo, .amap-demo {
    height: 300px;
  }

  .search-box {
    position: absolute !important;
    top: 25px;
    left: 20px;
  }

  .amap-page-container {
    position: relative;
  }

  .toolbar {
    margin-top: 10px;
  }
</style>

<script>
export default {
  data () {
    let self = this
    return {
      lng: 121.420866,
      lat: 28.655815,
      zoom: 12,
      center: [121.420866, 28.655815],
      address: '',
      loaded: false,
      searchOption: {
        city: '台州',
        citylimit: true
      },
      markers: [{
        position: [121.420866, 28.655815]
      }],
      plugin: [{
        pName: 'Geolocation',
        events: {
          init (o) {
            // o 是高德地图定位插件实例
            o.getCurrentPosition((status, result) => {
              if (result && result.position) {
                self.lng = result.position.lng
                self.lat = result.position.lat
                self.center = [self.lng, self.lat]
                self.removeMarker()
                self.addMarker()
                self.loaded = true
                // 这里通过高德 SDK 完成。
                var geocoder = new AMap.Geocoder({
                  radius: 1000,
                  extensions: 'all'
                })
                geocoder.getAddress(self.center, function (status, result) {
                  if (status === 'complete' && result.info === 'OK') {
                    if (result && result.regeocode) {
                      self.address = result.regeocode.formattedAddress
                      self.$nextTick()
                    }
                  }
                })
                self.$nextTick();
              }
            })
          }
        }
      }],
      events: {
        click (e) {
          let {lng, lat} = e.lnglat
          self.lng = lng
          self.lat = lat
          self.removeMarker()
          self.addMarker()
          // 这里通过高德 SDK 完成。
          var geocoder = new AMap.Geocoder({
            radius: 1000,
            extensions: 'all'
          })
          geocoder.getAddress([lng, lat], function (status, result) {
            if (status === 'complete' && result.info === 'OK') {
              if (result && result.regeocode) {
                self.address = result.regeocode.formattedAddress
                self.$nextTick()
              }
            }
          })
        }
      }
    }
  },
  methods: {
    handleClose () {
      this.$emit('selectMap', false)
    },
    done () {
      console.log({
        lng: this.lng,
        lat: this.lat
      })
      this.$emit('selectPosition', {
        lng: this.lng,
        lat: this.lat
      })
      this.handleClose()
    },
    onSearchResult (pois) {
      let latSum = 0
      let lngSum = 0
      if (pois.length > 0) {
        pois.forEach(poi => {
          let {lng, lat} = poi
          lngSum += lng
          latSum += lat
          // this.markers.push([poi.lng, poi.lat]);
        })
        let center = {
          lng: lngSum / pois.length,
          lat: latSum / pois.length
        }
        this.lng = center.lng
        this.lat = center.lat
        this.center = [center.lng, center.lat]
        this.removeMarker()
        this.addMarker()
      }
    },
    addMarker () {
      let marker = {
        position: [this.lng, this.lat]
      }
      this.markers.push(marker)
    },
    removeMarker () {
      if (!this.markers.length) return
      this.markers.splice(this.markers.length - 1, 1)
    }
  },
  props: ['mapShow']
}
</script>

<style scoped>

</style>
